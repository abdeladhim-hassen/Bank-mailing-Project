export interface UpdateNotificationDto {
  id: number;
  name: string;
  sendDate: Date;
  emailBody: string;
  smsMessage: string;
  whatsMessage: string;
}
