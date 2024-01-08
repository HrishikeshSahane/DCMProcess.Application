import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-physician-layout',
  templateUrl: './physician-layout.component.html',
  styleUrls: ['./physician-layout.component.css']
})
export class PhysicianLayoutComponent {
  constructor(private router: Router) { }
  ngOnInit() {
  }
  logOut() {
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    sessionStorage.clear();
    this.router.navigate(['']);
  }
}
