export interface MessageStream {
  chatId: number,
  authorId: number,
  authorName: string,
  date: Date,
  content: string,
  fromCurrentUser: boolean,
}

export interface ChatsStream {
  id: number, //id chatu
  offerId: number,
  offerTitle: string,
  offerPrice: string,
  offerImageUrl: string,
  withUser: string,
  date: Date,
  content: string
  isRead: boolean,

  isCurrentChat?: boolean,
  isNewChat?: boolean,
}
