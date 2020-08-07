import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/_models/photo';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  response: string;
  baseUrl = environment.apiUrl;
  currentMain: Photo;
 
  constructor (private auth: AuthService, private userService: UserService, private alertify: AlertifyService ) {
    this.hasBaseDropZoneOver = false;
 
    this.response = '';
 
    this.uploader.response.subscribe( res => this.response = res );
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.auth.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['images'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
      });

      this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

      this.uploader.onSuccessItem = (item, response, status, headers) => {
        if (response) {
          const res: Photo = JSON.parse(response);
          const photo = {
            id: res.id,
            url: res.url,
            dateAdded: res.dateAdded,
            description: res.description,
            isMain: res.isMain
          };
          this.photos.push(photo);
          if (photo.isMain) {
            this.auth.changeMemberPhoto(photo.url);
            this.auth.currentUser.photoUrl = photo.url;
            localStorage.setItem('user', JSON.stringify(this.auth.currentUser));
          }
        }
      }
  }

  setMainPhoto(photo: Photo) {
    this.userService.setMainPhoto(this.auth.decodedToken.nameId, photo.id).subscribe(() => {
      this.currentMain = this.photos.filter(x => x.isMain === true)[0];
      this.currentMain.isMain = false;
      photo.isMain = true;
      this.auth.changeMemberPhoto(photo.url);
      this.auth.currentUser.photoUrl = photo.url;
      localStorage.setItem('user', JSON.stringify(this.auth.currentUser));
    }, error => {
      this.alertify.error(error);
    }
    )
  }

  deletePhoto(id: number) { 
    this.alertify.confirm("Are you sure you want to delete this photo?", 
    () => { this.userService.deletePhoto(this.auth.decodedToken.nameId, id)
    .subscribe(() => { 
      this.photos.splice(this.photos.findIndex(x => x.id === id) , 1);
      this.alertify.success('Photo has been deleted');
    }, error => {
      this.alertify.error('Failed to delete the photo');
    })
  });
  }
}