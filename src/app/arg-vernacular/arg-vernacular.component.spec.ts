import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArgVernacularComponent } from './arg-vernacular.component';

describe('ArgVernacularComponent', () => {
  let component: ArgVernacularComponent;
  let fixture: ComponentFixture<ArgVernacularComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArgVernacularComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArgVernacularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
