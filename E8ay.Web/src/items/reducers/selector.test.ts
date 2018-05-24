import { getItemsState, getItemById, getItems, IItemsState } from './';
import { IAuctionItem, ItemStatus } from '../models';
import { IRootState } from '../../reducer';

describe('Items Reducer tests', () => {
  
  const testItem1: IAuctionItem = {
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
      
  const testItem2: IAuctionItem = {
    id: 'test_id_2',
    name: 'test_name_2',
    description: 'test_description_2',
    highestBiderId: 'test_bider_id_2',
    highestPrice: 13,
    status: ItemStatus.UnderOffer,
    endDateTime: 'test_end_date',
    startDateTime: 'test_start_date',
    bidPrice: 14
  }

  const testStae: IItemsState = {
    auctionItems: {
      byId: {
        [testItem1.id]: testItem1,
        [testItem2.id]: testItem2
      },
      ids: [testItem1.id, testItem2.id]
    }
  }

  const testRootState: IRootState = {
    items: testStae,
    login: {} as any,
    router: {} as any
  }

  it('test getItemsState', () => {
    const itemState = getItemsState(testRootState);
    expect(itemState).toEqual(testStae);
  })

  it('test getItems', () => {
    const items = getItems(testRootState);
    expect(items).toEqual([testItem1, testItem2]);
  })

  it('test getItemById', () => {
    const auctionItem = getItemById(testItem1.id)(testRootState);
    expect(auctionItem).toEqual(testItem1);
  })

})