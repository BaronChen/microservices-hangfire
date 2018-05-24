
import { combineReducers } from 'redux';
import { createSelector } from 'reselect'
import { ItemsAction, IUpdateItemInfo, IGetItems, actionTypes } from '../actions/items.actions';
import { IAuctionItem } from '../models';
import { IRootState } from '../../reducer';
import { handle, ActionMeta } from 'redux-pack';
import { IFlatArray, getFlatArray } from '../../common/models/flat-array';

export interface IItemsState {
  readonly auctionItems: IFlatArray<IAuctionItem>
};

export const auctionItemsReducer = (state: IFlatArray<IAuctionItem> = { byId: {}, ids: [] }, action: ItemsAction) => {
  switch (action.type) {
    case actionTypes.GET_ITEMS:
      return handle<IFlatArray<IAuctionItem>, any, {}, {}, ActionMeta<{}, {}, {}, {}>>(state, action, {
        success: prevState => getFlatArray((action as IGetItems).payload)
      });
    case actionTypes.UPDATE_ITEM_INFO:
      return getUpdatedItemState(state, (action as IUpdateItemInfo).payload);
    default: 
      return state;
  }
};

export const itemsReducer = combineReducers<IItemsState, ItemsAction>({
  auctionItems: auctionItemsReducer
});

export const getUpdatedItemState = (state: IFlatArray<IAuctionItem>, newItem: IAuctionItem) => {
  return Object.assign({}, state, { byId: Object.assign({}, state.byId, { [newItem.id]: newItem }) });
}

export const getItemsState = (state: IRootState):IItemsState => state.items;

export const getItems = createSelector<IRootState, IItemsState, IAuctionItem[]>(
  getItemsState,
  (state:IItemsState) => state.auctionItems.ids.map(x => state.auctionItems.byId[x])
);

export const getItemById = (id:string) => createSelector<IRootState, IItemsState, IAuctionItem>(
  getItemsState,
  (state:IItemsState) => state.auctionItems.byId[id]
);
