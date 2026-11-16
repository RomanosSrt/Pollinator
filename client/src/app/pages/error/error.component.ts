import { Component } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';

// Shown for any unknown URL.
@Component({
  selector: 'app-error',
  imports: [RouterLink, MatButton],
  templateUrl: './error.component.html',
})
export class ErrorComponent {}
