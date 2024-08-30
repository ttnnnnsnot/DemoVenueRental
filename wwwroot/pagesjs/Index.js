import { setupLayout, layoutOption } from '../pagesjs/Layout.js';

import indexBannerOption from '../components/IndexBanner.js';
import {
    indexSelectTypeOption,
    indexSelectMore,
    search as searchFunction
} from '../components/IndexSelectType.js';

const appOption = {
    components: {
        // Layout.js
        ...layoutOption.components,
        // Index.js
        'index-select-type': indexSelectTypeOption,
        'index-banner': indexBannerOption,
    },
    setup() {
        const instance = getCurrentInstance();
        // Layout.js
        const { 
            checkPathName,
            registerComponent, showRegisterModel,
            loginComponent, showLoginModel,
            Logouted,
            LoggedIn,
            LayoutonBeforeMount,
            LayoutonMounted,
            headerCurrentState } = setupLayout();

        onBeforeMount(async () => {
            await LayoutonBeforeMount();
            await indexSelectMoreonBeforeMount();
        });

        onMounted(async () => {
            await LayoutonMounted();
        });

        // Index.js
        const fetchData1 = () => API.GET('def/sport');
        const fetchData2 = () => API.GET('def/area');

        const {
            selectTypes,
            fnChangeText,
            onBeforeMount: indexSelectMoreonBeforeMount
        } = indexSelectMore([fetchData1, fetchData2]);

        const search = () => {
            checkPathName('/home/placeedit');
        }//searchFunction(selectTypes);

        return {
            // Layout.js
            registerComponent, showRegisterModel,
            loginComponent, showLoginModel,
            LoggedIn,
            Logouted,

            // Index.js
            fnChangeText,
            selectTypes,
            search,

        }
    }
};

const app = createApp(appOption);

app.mount("#app");