export interface Offers {
  name: string,
  data: MainOffers[],
}

export interface MainOffers {
  id: number,
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