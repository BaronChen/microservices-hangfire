jest.mock('react-s-alert');

jest.mock('../items.api', () => ({
  loadItems: jest.fn(),
  placeBid: jest.fn()
}));


import {
  actionTypes, 
  getItemsAction,
  updateItemInfo, 
  placeBidAction
} from './items.actions';

import { loadItems, placeBid } from '../items.api';
import { IAuctionItem, ItemStatus, IAuctionBid } from '../models';
import { IErrorModel } from '../../common/http/http-client';

import Alert from 'react-s-alert';

describe('Items actions tests', () => {
  it('Create IGetItems action correctly', () => {
    const action = getItemsAction();
    expect(action.type).toBe(actionTypes.GET_ITEMS);
    expect(action.promise).not.toBe(null);
    expect(action.payload).toBe(undefined);
    expect(loadItems).toHaveBeenCalledTimes(1);
    const testError = 'testError';
    (action.meta as any).onFailure(testError, ({} as any));
    expect(Alert.error).toHaveBeenCalledTimes(1);
    expect(Alert.error).toHaveBeenCalledWith(testError, expect.anything());
  });


  it('Create IUpdateItemInfo action correctly', () => {
    const testItem: IAuctionItem = {
      id: 'test_id',
      name: 'test_name',
      description: 'test_description',
      highestBiderId: 'test_bider_id',
      highestPrice: 12,
      status: ItemStatus.Sold,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 13
    }
    const action = updateItemInfo(testItem);
    expect(action.type).toBe(actionTypes.UPDATE_ITEM_INFO);
    expect(action.payload).toBe(testItem);
  });

  it('Create IPlaceBid action correctly', () => {
    
    const testBid: IAuctionBid = {
      userId: "test_user_id",
      itemId: "test_item_id",
      bidPrice: 10
    }
    const action = placeBidAction(testBid.itemId, testBid.userId, testBid.bidPrice);
    expect(action.type).toBe(actionTypes.PLACE_BID);
    expect(placeBid).toHaveBeenCalledTimes(1);
    expect(placeBid).toBeCalledWith(testBid);

    const testError:IErrorModel = {
      status: 400,
      errors: ['testError']
    };

    (Alert as any).error.mockReset();
    (action.meta as any).onFailure(testError, ({} as any));
    expect(Alert.error).toHaveBeenCalledTimes(1);
    expect(Alert.error).toHaveBeenCalledWith(testError.errors[0], expect.anything());

    (Alert as any).success.mockReset();
    (action.meta as any).onSuccess('', ({} as any));
    expect(Alert.success).toHaveBeenCalledTimes(1);
    expect(Alert.success).toHaveBeenCalledWith('Bid Placed', expect.anything())

  });

})

