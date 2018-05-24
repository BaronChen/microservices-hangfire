jest.mock('redux-pack', () => ({
  handle: jest.fn()
}));


import { auctionItemsReducer } from './';
import { IGetItems, IUpdateItemInfo, actionTypes } from '../actions/items.actions';

import {handle} from 'redux-pack';
import { IAuctionItem, ItemStatus } from '../models';
import { IFlatArray } from '../../common/models/flat-array';

describe('Items Reducer tests', () => {

  it('should handle GET_ITEMS correctly', () => {
    const initialState = { byId: {}, ids: [] };
    
    const testAction: IGetItems = {
      type: actionTypes.GET_ITEMS,
      promise: {} as any,
      payload: [],
      meta: {} as any
    }

    auctionItemsReducer(initialState, testAction);

    expect(handle).toHaveBeenCalledTimes(1);
    expect(handle).toHaveBeenCalledWith(initialState, testAction, expect.anything());
  });

  it('should handle UPDATE_ITEM_INFO correctly', () => {
    const testItem: IAuctionItem = {
      id: 'test_id',
      name: 'test_name',
      description: 'test_description',
      highestBiderId: 'test_bider_id',
      highestPrice: 12,
      status: ItemStatus.Listed,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 13
    }
    const initialState: IFlatArray<IAuctionItem> = { 
      byId: {
        [testItem.id]: testItem
      }, 
      ids: [testItem.id] 
    };
    
    const newItem: IAuctionItem = {
      id: 'test_id',
      name: 'test_name_2',
      description: 'test_description_2',
      highestBiderId: 'test_bider_id_2',
      highestPrice: 13,
      status: ItemStatus.UnderOffer,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 14
    }

    const testAction: IUpdateItemInfo = {
      type: actionTypes.UPDATE_ITEM_INFO,      
      payload: newItem
    }

    const newState = auctionItemsReducer(initialState, testAction);
    expect(newState).not.toBe(initialState);
    expect(newState.byId[testItem.id]).toEqual(newItem);
  });

})
