export interface Message {
    id: number
    senderId: number
    senderUsername: string
    senderPhotoUrl: string
    recipientId: number
    recipienUsername: string
    recipientPhotoUrl: string
    content: string
    dateRead?: Date;
    messageSent: Date;
  }