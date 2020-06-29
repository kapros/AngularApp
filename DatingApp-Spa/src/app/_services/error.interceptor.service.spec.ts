import { TestBed } from '@angular/core/testing';

import { Error.InterceptorService } from './error.interceptor.service';

describe('Error.InterceptorService', () => {
  let service: Error.InterceptorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Error.InterceptorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
