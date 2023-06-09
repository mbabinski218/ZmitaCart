export interface SingleOffer {
  id: number,
  title: string,
  description: string,
  price: number,
  quantity: number,
  isAvailable: boolean,
  createdAt: Date,
  condition: string,
  user: {
    email: string,
    firstName: string,
    lastName: string,
    phoneNumber: string,
  },
  address: {
    country: string,
    city: string,
    street: string,
    postalCode: number,
    houseNumber: number,
    apartmentNumber: number,
  },
  picturesNames: [
    string,
  ],
  isFavourite: boolean,
}