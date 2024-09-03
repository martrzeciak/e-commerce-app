export type User = {
    firstName: string;
    lastName: string;
    email: string;
    address: Addresss;
}

export type Addresss = {
    line1: string;
    line2?: string;
    city: string;
    state: string;
    country: string;
    postalCode: string;
}