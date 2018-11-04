import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArgumentDetailComponent } from './argument-detail.component';

describe('ArgumentDetailComponent', () => {
  let component: ArgumentDetailComponent;
  let fixture: ComponentFixture<ArgumentDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArgumentDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArgumentDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
