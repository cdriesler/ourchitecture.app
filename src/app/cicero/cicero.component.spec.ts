import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CiceroComponent } from './cicero.component';

describe('CiceroComponent', () => {
  let component: CiceroComponent;
  let fixture: ComponentFixture<CiceroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CiceroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CiceroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
