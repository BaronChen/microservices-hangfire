import { get, post } from '../common/http/http-client';
import { IAuctionItem, IAuctionBid } from './models';

export const loadItems = async ():Promise<IAuctionItem[]> => {
  
  try {
    const result = await get<IAuctionItem[]>('http://localhost:8200/api/items', true);
    return result || [];
  }catch(error) {
    throw JSON.stringify(error);
  }
}

export const placeBid = async (data: IAuctionBid):Promise<null> => { 
  try {
    await post<null>('http://localhost:8300/api/bids/add', data ,true);
    return null;
  }catch(error) {
    throw JSON.stringify(error);
  }
}