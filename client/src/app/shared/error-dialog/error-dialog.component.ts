import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogTitle } from '@angular/material/dialog';

export interface ErrorDialogData {
  message: string;
  traceId: string | null;
}

// Modal shown by ErrorService.
@Component({
  selector: 'app-error-dialog',
  imports: [MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose, MatButton],
  template: `
    <h2 mat-dialog-title>Something went wrong</h2>
    <mat-dialog-content>
      <p>{{ data.message }}</p>
      @if (data.traceId) {
        <p class="text-xs opacity-60">Reference: {{ data.traceId }}</p>
      }
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button matButton="filled" mat-dialog-close>Close</button>
    </mat-dialog-actions>
  `,
})
export class ErrorDialogComponent {
  protected readonly data = inject<ErrorDialogData>(MAT_DIALOG_DATA);
}
