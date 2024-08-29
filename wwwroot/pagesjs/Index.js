﻿import { setupLayout, layoutOption } from '../pagesjs/Layout.js';

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
        // Layout.js
        const { registerComponent, showRegisterModal,
            loginComponent, showLoginModal,
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

        const search = () => searchFunction(selectTypes);


        return {
            // Layout.js
            showRegisterModal, registerComponent,
            loginComponent, showLoginModal,
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