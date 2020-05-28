import {DialogController} from 'aurelia-dialog';
  
  export class Popup {
    static inject = [DialogController];
    message = "";
    controller: any;
    constructor(controller){
      this.controller = controller;
    }
    activate(message){
      this.message = message;
    }
  }
  

  