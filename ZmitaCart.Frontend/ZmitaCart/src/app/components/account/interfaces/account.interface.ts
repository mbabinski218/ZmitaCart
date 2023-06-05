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
