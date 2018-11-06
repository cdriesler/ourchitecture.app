import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArgAnalyticComponent } from './arg-analytic.component';

describe('ArgAnalyticComponent', () => {
  let component: ArgAnalyticComponent;
  let fixture: ComponentFixture<ArgAnalyticComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArgAnalyticComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArgAnalyticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
