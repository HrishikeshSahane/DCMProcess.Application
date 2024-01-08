import { TestBed } from '@angular/core/testing';

import { IuDicomwebviewerService } from './iu-dicomwebviewer.service';

describe('IuDicomwebviewerService', () => {
  let service: IuDicomwebviewerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IuDicomwebviewerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
