export interface CredentialsForm {
  phoneNumber: string,
  address: {
    country: string,
    city: string,
    street: string,
    postalCode: number,
    houseNumber: number,
    apartmentNumber: number,
  }
}

export interface UserCredentials {
  email: string,
  firstName: string,
  lastName: string,
  phoneNumber: string,
  address: {
    country: string,
    city: string,
    street: string,
    postalCode: number,
    houseNumber: number,
    apartmentNumber: number,
  }
}

export interface UserCredentialsShow {
  name: string,
  value: string | number,
  icon: string,
}

export interface Chats {
  items: [
    {
      conversationId: number,
      withUser: string,
      lastMessage: string,
      lastMessageCreatedAt: Date,
      offerId: number,
      offerTitle: string,
      offerImageUrl: string,
      offerPrice: string,
    },
  ],
  pageNumber: number,
  totalPages: number,
  totalCount: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
}

export interface FavouriteOffers {
  items: FavouriteItem[],
  pageNumber: number,
  totalPages: number,
  totalCount: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
}

export interface FavouriteItem {
  id: number,
  title: string,
  price: number,
  city: string,
  condition: string,
  quantity: number,
  imageName: string,
  isFavourite: boolean,
  isAvailable: boolean,
  authorName: string,
  authorEmail: string,
}

export interface SingleChat {
  withUser: string,
  offerId: number,
  offerTitle: string,
  offerImageUrl: string,
  offerPrice: string,
  conversationId?: number,
  lastMessage?: string,
  lastMessageCreatedAt?: Date,
}