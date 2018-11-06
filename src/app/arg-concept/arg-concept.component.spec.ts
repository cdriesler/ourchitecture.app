import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArgConceptComponent } from './arg-concept.component';

describe('ArgConceptComponent', () => {
  let component: ArgConceptComponent;
  let fixture: ComponentFixture<ArgConceptComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArgConceptComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArgConceptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
