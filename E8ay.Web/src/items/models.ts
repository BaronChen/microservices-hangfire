export enum ItemStatus {
  Listed = 0,
  UnderOffer = 1,
  Sold = 2
}

export interface IItem {
  id: string,
  name: string,
  description: string,
  status: ItemStatus,
  endDateTime: string,
  startDateTime: string,
  highestPrice: number,
  highestBiderId: string
}