import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArgPrecedentsComponent } from './arg-precedents.component';

describe('ArgPrecedentsComponent', () => {
  let component: ArgPrecedentsComponent;
  let fixture: ComponentFixture<ArgPrecedentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArgPrecedentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArgPrecedentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
