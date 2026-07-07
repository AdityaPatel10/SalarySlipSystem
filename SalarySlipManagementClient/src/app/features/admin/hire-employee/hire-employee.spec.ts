import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HireEmployee } from './hire-employee';

describe('HireEmployee', () => {
  let component: HireEmployee;
  let fixture: ComponentFixture<HireEmployee>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HireEmployee],
    }).compileComponents();

    fixture = TestBed.createComponent(HireEmployee);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
