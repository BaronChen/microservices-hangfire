import { get, post } from '../common/http/http-client';
import { IAuctionItem, IAuctionBid } from './models';
import { itemServiceUrl, bidServiceUrl } from '../common/config';

export const loadItems = async ():Promise<IAuctionItem[]> => {
  
  try {
    const result = await get<IAuctionItem[]>(`${itemServiceUrl}/api/items`, true);
    return result || [];
  }catch(error) {
    throw JSON.stringify(error);
  }
}

export const placeBid = async (data: IAuctionBid):Promise<null> => { 
  try {
    await post<null>(`${bidServiceUrl}/api/bids/add`, data ,true);
    return null;
  }catch(error) {
    throw JSON.stringify(error);
  }
}