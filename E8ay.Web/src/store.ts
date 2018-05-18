import { routerReducer, RouterState } from 'react-router-redux';

import { applyMiddleware, combineReducers, compose, createStore } from 'redux';


export interface IRootState {
  router: RouterState;
}

export const rootReducer = combineReducers<IRootState>({
  router: routerReducer
});

declare var window: any;

const composeEnhancers =
  typeof window === 'object' &&
  window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ ?   
    window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__({ }) : compose;

const configureStore = (initialState?: IRootState) => {
  // configure middlewares
  const middlewares = new Array<any>();
  // compose enhancers
  const enhancer = composeEnhancers(
    applyMiddleware(...middlewares)
  );
  // create store
  return createStore(
    rootReducer,
    initialState!,
    enhancer
  );
}

// pass an optional param to rehydrate state on app start
const store = configureStore();

// export store singleton instance
export default store;