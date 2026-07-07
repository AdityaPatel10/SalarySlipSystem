import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditEmployeeModal } from './edit-employee-modal';

describe('EditEmployeeModal', () => {
  let component: EditEmployeeModal;
  let fixture: ComponentFixture<EditEmployeeModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditEmployeeModal],
    }).compileComponents();

    fixture = TestBed.createComponent(EditEmployeeModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
