import { TestBed } from '@angular/core/testing';

import { SalarySlip } from './salary-slip';

describe('SalarySlip', () => {
  let service: SalarySlip;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SalarySlip);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
