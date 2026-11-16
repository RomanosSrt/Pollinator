import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { ErrorDialogComponent } from '../../shared/error-dialog/error-dialog.component';
import { ApiResponse } from '../models/api-response.model';

// Shows backend failures to the user in a modal.
@Injectable({ providedIn: 'root' })
export class ErrorService {
  private readonly dialog = inject(MatDialog);

  show(err: HttpErrorResponse): void {
    if (this.dialog.openDialogs.length) return; // one modal at a time
    const traceId = (err.error as Partial<ApiResponse<unknown>> | null)?.traceId ?? null;
    this.dialog.open(ErrorDialogComponent, { data: { message: describe(err.status), traceId } });
  }
}

// Maps an HTTP status to a user-friendly message.
function describe(status: number): string {
  if (status === 0) return 'The server cannot be reached. Check your connection and try again.';
  if (status === 400) return 'The server rejected the submitted data. Please check it and try again.';
  if (status === 401) return 'Your session has expired. Please sign in again.';
  if (status === 403) return 'You do not have permission to do that.';
  if (status === 404) return 'The requested item was not found.';
  return 'Something went wrong on the server. Please try again in a moment.';
}
