import { Store, AnyAction} from 'redux';
import { IRootState } from './reducer';


class StoreRegistry {

  private store: Store<IRootState, AnyAction>;
  
  public registerStore(store:Store<IRootState, AnyAction>) {
    this.store = store;
  }

  public getStore():Store<IRootState, AnyAction> {
    return this.store;
  }

}

export const storeRegistry = new StoreRegistry();




