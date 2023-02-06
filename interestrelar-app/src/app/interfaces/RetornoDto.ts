export interface RetornoDto {
  message: string;
  success: boolean;
  statusCode: number;
  totalItems: number;
  page: string;
  data: any;
}
