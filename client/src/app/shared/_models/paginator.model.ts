import { PageEvent } from "@angular/material/paginator";

export class PaginatorModel {
    pageIndex: number = 0;
    pageSize: number = 30;
    pageSizeOptions = [5, 15, 30];
    length: number = 0;
    pageEvent: PageEvent;
    // sort?: string = null;
    // direction?: string = null;



    //    length = 50;
    //   pageSize = 30;
    //   pageIndex = 0;
    //   pageSizeOptions = [5, 15, 30];
    //   pageEvent: PageEvent;
}