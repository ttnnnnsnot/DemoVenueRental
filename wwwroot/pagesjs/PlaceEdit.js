import { setupLayout, layoutOption } from '../pagesjs/Layout.js';

const appOption = {
    components: {
        // Layout.js
        ...layoutOption.components,
    },
    setup() {
        // Layout.js
        const {
            registerComponent,
            loginComponent,
            isLoggedIn,
            layoutLogouted,
            layoutLoggedIn,
            layoutOnBeforeMount,
            layoutOnMounted,
            headerCurrentState,
            checkPathName } = setupLayout();

        onBeforeMount(async () => {
            await layoutOnBeforeMount();
        });

        onMounted(async () => {
            await layoutOnMounted();
            // PlaceEdit.js
            changeHeaderState();
            setActiveTabList();
            changeTab(activeTab.value);
        });

        const performLoggedIn = async () => {
            await layoutLoggedIn();

            // PlaceManage.js
            changeHeaderState();
        }

        const performLogouted = async () => {
            await layoutLogouted();

            // PlaceManage.js
            changeHeaderState();
        }

        // PlaceEdit.js
        const activeTabList = ref([]);
        const activeTab = ref(0);

        const setActiveTabList = () => {
            const buttons = document.querySelectorAll('#nav-tab > .nav-link');
            activeTabList.value = Array.from(buttons).map(button => `#${button.id}`);
        };

        const changeTab = (index) => {
            if (index !== undefined && activeTabList.value.length > index && index >= 0)
            {
                activeTab.value = index;
            }
            document.querySelector(activeTabList.value[activeTab.value]).click();
        }

        const nextDone = async () => {
            if (await sendForm()) {
                if (activeTab.value < activeTabList.value.length - 1) {
                    changeTab(++activeTab.value);
                }
            }
        }

        const upDone = () => {
            if (activeTab.value > 0) {
                changeTab(--activeTab.value);
            }
        }

        const changeHeaderState = () => {
            headerCurrentState.value = isLoggedIn.value ? 3 : 1;
        }

        const sendForm = async () => {
            const forms = document.querySelectorAll('#PlaceEditTab form');
            const thisform = forms[activeTab.value];

            if (!$(thisform).valid())
                return false;
            
            const formData = new FormData(thisform);

            const data = Object.fromEntries(
                Array.from(formData.entries())
            );

            try {
                const results = await API.POST("Place", data);
                return results.state;

            } catch (error) {
                console.log('Error:', error);
                return false;
            }
        }

        // Layout.js
        provide('loggedIn', performLoggedIn);
        provide('logouted', performLogouted);

        return {
            // Layout.js
            registerComponent,
            loginComponent,

            // PlaceEdit.js
            nextDone,
            upDone
        }
    }
};

const app = createApp(appOption);

app.mount("#app");