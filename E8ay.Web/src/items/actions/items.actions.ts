import { Action } from 'redux';
import { IRootState } from '../../store';
import { GetState, ActionMeta } from 'redux-pack';
import { IItem } from '../models';
 
export const actionTypes = {
  GET_ITEMS: '[ITEMS]GET_ITEMS',
  UPDATE_ITEM_INFO: '[ITEMS]UPDATE_ITEM_INFO'
}


export interface IGetItems extends Action {
  payload?: IItem[],
  promise: Promise<any>,
  meta: ActionMeta
}

export interface IUpdateItemInfo extends Action {
  payload: IItem 
}

export type LoginActions = IGetItems | IUpdateItemInfo;

export const getItems = (): IGetItems => ({
  type: actionTypes.GET_ITEMS,
  promise: Promise.reject('temp'),
  meta: {
    onFailure: (error: string, getState: GetState<IRootState>) => {
      alert(error);
    }
  }
})

export const updateItemInfo = (item: IItem): IUpdateItemInfo => ({
  type: actionTypes.UPDATE_ITEM_INFO,
  payload: item
})

