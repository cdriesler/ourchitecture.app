import { TestBed } from '@angular/core/testing';

import { DashboardRouteService } from './dashboard-route.service';

describe('DashboardRouteService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DashboardRouteService = TestBed.get(DashboardRouteService);
    expect(service).toBeTruthy();
  });
});
