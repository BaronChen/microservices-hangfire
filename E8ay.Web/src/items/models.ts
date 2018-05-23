import { IFlatArrayItem } from '../common/models/flat-array';

export enum ItemStatus {
  Listed = 0,
  UnderOffer = 1,
  Sold = 2,
  End = 3
}

export interface IAuctionItem extends IFlatArrayItem {
  readonly name: string,
  readonly description: string,
  readonly status: ItemStatus,
  readonly endDateTime: string,
  readonly startDateTime: string,
  readonly highestPrice: number,
  readonly highestBiderId: string,
  readonly bidPrice: number,
}

export interface IAuctionBid {
  userId: string,
  itemId: string,
  bidPrice: number
}