import { Component, OnInit } from '@angular/core';
import {
  PostRouteService,
  PostShipService,
  GetShipService,
  GetPointService,
} from 'src/app/shared/services';
import { Point, RouteModel, ShipModel } from 'src/app/shared/models';
import { v4 as uuidv4 } from 'uuid';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-list-ships-page',
  templateUrl: './list-ships-page.component.html',
  styleUrls: ['./list-ships-page.component.scss'],
})
export class ListShipsPageComponent implements OnInit {
  PointData: Point[] = [];
  routeData: RouteModel[] = [];
  start: string = '';
  finish: string = '';
  routes: RouteModel = new RouteModel();

  ship: string = '';
  parsedShipsData: ShipModel[] = [];
  selectedShip: ShipModel = new ShipModel();

  constructor(
    private routeService: PostRouteService,
    private shipService: PostShipService,
    private getShipService: GetShipService,
    private getPointService: GetPointService
  ) {}

  ngOnInit(): void {
    this.getPointService.getPoints().subscribe({
      next: (points: Point[]) => {
        this.PointData = points;
        console.log(points)
      },
      error: (error) => {
        console.error(error);
      },
    });
    this.getShipService.getShips().subscribe({
      next: (ships: ShipModel[]) => {
        this.parsedShipsData = ships;
      },
      error: (error) => {
        console.error(error);
      },
    });
    this.routeService.getRoute().subscribe({
      next: (routes: RouteModel[]) => {
        console.log(routes)
        this.routeData = routes;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  public async postRoute(routes: RouteModel) {
    let t = this;

    await lastValueFrom(t.routeService.PostRoute(routes))
      .then((res) => {})
      .catch((e) => {
        console.error('Ошибка при загрузке маршрута', e);
      });
  }

  public async postShip(ship: ShipModel) {
    let t = this;

    await lastValueFrom(t.shipService.PostShip(ship))
      .then((res) => {})
      .catch((e) => {
        console.error('Ошибка при загрузке маршрута', e);
      });
  }

  sendRoute = (): void => {
    let t = this;
    const selectedStartPoint = t.PointData.find(
      (point) => point.graphDataId === t.start
    );
    const selectedEndPoint = t.PointData.find(
      (point) => point.graphDataId === t.finish
    );
    const selectedShip = t.parsedShipsData.find(
      (ship) => ship.shipName === t.ship
    );

    if (selectedStartPoint && selectedEndPoint && selectedShip) {
      const route = {
        id: uuidv4(),
        startId: selectedStartPoint.graphDataId,
        finishId: selectedEndPoint.graphDataId,
      };
      const currentShip = {
        shipName: selectedShip.shipName,
        speed: selectedShip.speed,
        shipClassId: selectedShip.shipClassId,
      };

      t.routes = route;
      t.selectedShip = currentShip;
      t.start = '';
      t.finish = '';
      t.ship = '';

      t.postRoute(t.routes);
      t.postShip(t.selectedShip);
    }
  };
}
