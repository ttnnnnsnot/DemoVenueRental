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
            await getAreaList();
        });

        onMounted(async () => {
            await layoutOnMounted();
            // PlaceEdit.js
            changeHeaderState();
            setActiveTabList();
            changeTab(activeTab.value);
            listenerButtons();
            await getPlaceInfo();
            await getSportList();
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
        const placeId = ref(0);
        const activeTabList = ref([]);
        const activeTab = ref(1);
        const placeInfo = ref({
            placeInfo: {
                name: '',
                cityId: 1,
                address: '',
                describe: '',
                rules: ''
            },
            placeImg: [],
            placeType: [],
        });

        const sportTypeList = ref([]);
        const sportTypeMessage = ref('');

        const areaList = ref([]);

        const getAreaList = async () => {
            const result = await API.GET('Def/area');
            if (result.state) {
                areaList.value = result.data;
            }
        }

        const sportList = ref([]);

        const getSportList = async () => {
            const result = await API.GET('Def/sport');
            if (result.state) {
                sportList.value = result.data;
            }
        }

        const getPlaceInfo = async () => {
            const results = await API.GET(`Place/PlaceInfo`);
            if (results.state) {
                placeInfo.value = results.data;

                sportTypeList.value = placeInfo.value.placeType.map(item => item.selectTypeId);
                placeId.value = placeInfo.value.placeInfo.placeId;
            }
        }

        const listenerButtons = () => {
            // 監聽 #nav-tab > .nav-link 的按鈕，並且設定 activeTabList
            const buttons = document.querySelectorAll('#nav-tab > .nav-link');
            buttons.forEach((button, index) => {
                button.addEventListener('click', () => {
                    activeTab.value = index;
                });
            });
        }

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

            if (activeTab.value === 0) {
                return await SendPlace(thisform);
            } else if (placeId.value === 0) {
                await SendPlace(forms[0]);
            }

            if (placeId.value === 0) {
                Alert.addDanger("資訊有誤，請更新場所資訊，再進行下一步");
                return false;
            }

            if (activeTab.value === 1) {
                return await SendSportType(thisform);
            }
            
        }

        const SendSportType = async (thisform) => {
            
            if (sportTypeList.value.length === 0) {
                return false;
            }

            const data = sportTypeList.value;

            try {
                const results = await API.PUT("Place/SportType", data);

                if (!results.state) {
                    Alert.addDanger(results.message);
                    return false;
                }

                return true;
            } catch (error) {
                console.log('Error:', error);
                return false;
            }
        }

        // 監聽 sportTypeList 的變化
        watch(sportTypeList, (newVal) => {
            if (newVal.length === 0) {
                sportTypeMessage.value = '請至少選擇一個運動類型';
            } else {
                sportTypeMessage.value = '';
            }
        });
        

        const SendPlace = async (thisform) => {
            if (!$(thisform).valid())
                return false;

            const formData = new FormData(thisform);

            const data = Object.fromEntries(
                Array.from(formData.entries()).map(([key, value]) => [key.replace('PlaceInfo.',''), value])
            );

            try {
                const results = placeId.value === 0 ? await API.POST("Place", data) : await API.PUT("Place", data);
                    
                if (results.state)
                    placeId.value = results.data.placeId;
                else
                    Alert.addDanger(results.message);
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
            upDone,
            placeInfo,
            areaList,

            sportList,
            sportTypeList,
            sportTypeMessage
        }
    }
};

const app = createApp(appOption);

app.mount("#app");