export interface MainOffers {
  id: number,
  categoryName: string,
  title: string,
  price: number,
  address: {
    country: string,
    city: string,
    street: string,
    postalCode: number,
    houseNumber: number,
    apartmentNumber: number,
  },
  condition: string,
  quantity: number,
  imageName: string,
  isFavourite: boolean,
}