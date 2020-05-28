import { PLATFORM } from 'aurelia-pal';
import { RouterConfiguration, Router } from 'aurelia-router';

export class App {
  router;
  configureRouter(config: RouterConfiguration, router: Router): void {
    this.router = router;
    config.map([
      { route: [''], name: 'home', nav: true, moduleId: PLATFORM.moduleName('./components/applicant-form') }
      ,
      { route: ['success'], name: 'success', nav: true, moduleId: PLATFORM.moduleName('./components/success') }]);
  }
}
