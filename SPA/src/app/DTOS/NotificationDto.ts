export interface NotificationDto {
  id: number;
  name: string;
  dateCreated: Date;
  sendDate: Date;
  emailBody: string;
  smsMessage: string;
  whatsMessage: string;
}
