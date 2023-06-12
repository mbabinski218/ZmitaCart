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
  items: SingleChat[],
  pageNumber: number,
  totalPages: number,
  totalCount: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
}

export interface AccountOffers {
  items: OfferItem[],
  pageNumber: number,
  totalPages: number,
  totalCount: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
}

export interface OfferItem {
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
  id?: number,
  lastMessage?: string,
  lastMessageCreatedAt?: Date,

  hidden?: boolean,
  forcedToHistory?: boolean,
}