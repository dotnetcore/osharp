import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';

@Component({
    selector: 'dialog-result-example-dialog',
    template: `
        <h1 mat-dialog-title>Dialog</h1>
        <div mat-dialog-content>What would you like to do?</div>
        <div mat-dialog-actions>
          <button mat-button (click)="dialogRef.close('Option 1')">Option 1</button>
          <button mat-button (click)="dialogRef.close('Option 2')">Option 2</button>
        </div>
    `,
})
export class DialogResultExampleDialog {
    constructor(public dialogRef: MatDialogRef<DialogResultExampleDialog>) { }
}
