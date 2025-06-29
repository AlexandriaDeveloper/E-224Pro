export class Daily {
  id?: number;
  name: string;
  dailyDate: Date;
  dailyType: string;
  accountItem: string;

}

export class ReportRequest {
  startDate?: Date;
  endDate?: Date;
  dailyType?: string;
  collageId?: number;
  fundId?: number;
  entryType?: number
}
