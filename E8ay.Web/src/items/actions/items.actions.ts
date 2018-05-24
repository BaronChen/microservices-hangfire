import { Action } from 'redux';
import { IRootState } from '../../reducer';
import { GetState, ActionMeta } from 'redux-pack';
import { IAuctionItem } from '../models';
import { loadItems, placeBid } from '../items.api';
import { IErrorModel } from '../../common/http/http-client';
import Alert from 'react-s-alert';

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
      Alert.error(error, {
        position: 'top',
        effect: 'slide',
        timeout: 'none'
      });
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
      onFailure: (error: IErrorModel, getState: GetState<IRootState>) => {
        error.errors.map((x) => {
          Alert.error(x, {
            position: 'top',
            effect: 'slide',
            timeout: 'none'
          });
        })
      },
      onSuccess: (response:any, getState: GetState<IRootState>) => {
        Alert.success('Bid Placed', {
          position: 'top',
          effect: 'slide',
          timeout: 'none'
        });
      }
    }
  }
)