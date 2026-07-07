import { TestBed } from '@angular/core/testing';

import { SalaryStructure } from './salary-structure';

describe('SalaryStructure', () => {
  let service: SalaryStructure;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SalaryStructure);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
