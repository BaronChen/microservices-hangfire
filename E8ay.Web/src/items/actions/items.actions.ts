import { Action } from 'redux';
import { IRootState } from '../../reducer';
import { GetState, ActionMeta } from 'redux-pack';
import { IAuctionItem } from '../models';
import { loadItems, placeBid } from '../items.api';

export const actionTypes = {
  GET_ITEMS: '[ITEMS]GET_ITEMS',
  UPDATE_ITEM_INFO: '[ITEMS]UPDATE_ITEM_INFO',
  PLACE_BID: '[ITEMS]PLACE_BID'
}

export interface IGetItems extends Action {
  payload?: IAuctionItem[],
  promise: Promise<any>,
  meta: ActionMeta
}

export interface IUpdateItemInfo extends Action {
  payload: IAuctionItem
}

export interface IPlaceBid extends Action {
  promise: Promise<any>,
  meta: ActionMeta
}

export type ItemsAction = IGetItems | IUpdateItemInfo;

export const getItemsAction = (): IGetItems => ({
  type: actionTypes.GET_ITEMS,
  promise: loadItems(),
  meta: {
    onFailure: (error: string, getState: GetState<IRootState>) => {
      alert(error);
    }
  }
})

export const updateItemInfo = (item: IAuctionItem): IUpdateItemInfo => ({
  type: actionTypes.UPDATE_ITEM_INFO,
  payload: item
})

export const placeBidAction = (itemId: string, userId: string, bidPrice: number): IPlaceBid => (
  {
    type: actionTypes.PLACE_BID,
    promise: placeBid({
      userId,
      itemId,
      bidPrice
    }),
    meta: {
      onFailure: (error: string, getState: GetState<IRootState>) => {
        alert(error);
      },
      onSuccess: (response:any, getState: GetState<IRootState>) => {
        alert('Bid Placed');
      }
    }
  }
)