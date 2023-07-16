import { makeAutoObservable, runInAction } from "mobx";
import { User, UserFormValues } from "../models/user";
import agent from "../api/agent";
import { store } from "./store";
import { router } from "../router/Routes";

export default class UserStore {
    user: User | null = null;

    constructor() {
        makeAutoObservable(this)
    }

    get isLoggedIn() {
        return !!this.user; //this.user bir kullanıcı nesnesini referanslıyorsa, true değerini döndürür. Aksi takdirde, yani this.user null veya undefined ise, false değerini döndürür.
    }
   

    login = async (creds: UserFormValues) => {
        try {
            const user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user)
            router.navigate('/activities');
            store.modalStore.closeModal();
        } catch(error) {
            throw error;
        }
    }

    register = async (creds: UserFormValues) => {
        try {
            const user = await agent.Account.register(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user)
            router.navigate('/activities');
            store.modalStore.closeModal();
        } catch(error) {
            throw error;
        }

    }

    logout = () => {
        store.commonStore.setToken(null);
        this.user = null;
        router.navigate('/');
    }

    

    getUser = async() => {
        const user = await agent.Account.current();
        runInAction(() => this.user = user);
    }
}
